using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.TransactionReceipts;
using Nethereum.Web3;
using BadServer.DataBase;
using BadServer.DataBase.Entities;
using BadServer.Servicios;
using Microsoft.EntityFrameworkCore;


namespace Pago.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagoController : ControllerBase
    {
        private const string OUR_WALLET = "0xEa2755438EffDe69D7Dd4E01e9EF9264eF85aFaE";
        private const string NETWORK_URL = "https://rpc.sepolia.org";

        private readonly MyDbContext _dbContext;

        public PagoController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("products")]
        public IEnumerable<Producto> Get()
        {
            return _dbContext.Productos;
        }

        [HttpPost("buy")]
        public async Task<TransactionToSing> BuyAsync([FromForm] string clientWallet, [FromForm] decimal totalPrice)
        {
            using CoinGeckoApi coincGeckoApi = new CoinGeckoApi();
            decimal ethereumEur = await coincGeckoApi.GetEthereumPriceAsync();
            BigInteger priceWei = Web3.Convert.ToWei(totalPrice / ethereumEur);


            Web3 web3 = new Web3(NETWORK_URL);

            TransactionToSing transactionToSing = new TransactionToSing()
            {
                From = clientWallet,
                To = OUR_WALLET,
                Value = new HexBigInteger(priceWei).HexValue,
                Gas = new HexBigInteger(30000).HexValue,
                GasPrice = (await web3.Eth.GasPrice.SendRequestAsync()).HexValue
            };

            Transaction transaction = new Transaction()
            {
                ClientWallet = transactionToSing.From,
                Value = transactionToSing.Value,
                Hash = ""
            };

            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            transactionToSing.Id = transaction.Id;

            return transactionToSing;
        }

        // Clase para representar el cuerpo de la solicitud que incluye el precio total en Ethereum
        public class PrecioTotalBody
        {
            public string ClientWallet { get; set; }
            public BigInteger PrecioTotal { get; set; } // Aquí almacenaremos el precio total en Ethereum
        }


        [HttpPost("check/{transactionId}")]
        public async Task<bool> CheckTransactionAsync(int transactionId, [FromBody] string txHash)
        {
            bool success = false;
            Transaction transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
            
            transaction.Hash = txHash;
            

            Web3 web3 = new Web3(NETWORK_URL);
            var receiptPollingService = new TransactionReceiptPollingService(
                web3.TransactionManager, 1000);

            try
            {
                // Esperar a que la transacción se confirme en la cadena de bloques
                var transactionReceipt = await receiptPollingService.PollForReceiptAsync(txHash);

                // Obtener los datos de la transacción
                var transactionEth = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(txHash);

                Console.WriteLine(transactionEth.TransactionHash == transactionReceipt.TransactionHash);
                Console.WriteLine(transactionReceipt.Status.Value == 1);
                Console.WriteLine(transactionReceipt.TransactionHash == transaction.Hash);
                Console.WriteLine(transactionReceipt.From == transaction.ClientWallet);
                Console.WriteLine(transactionReceipt.To.Equals(OUR_WALLET, StringComparison.OrdinalIgnoreCase));

                success = transactionEth.TransactionHash == transactionReceipt.TransactionHash
                    && transactionReceipt.Status.Value == 1 // Transacción realizada con éxito
                    && transactionReceipt.TransactionHash == transaction.Hash // El hash es el mismo
                    && transactionReceipt.From == transaction.ClientWallet // El dinero viene del cliente
                    && transactionReceipt.To.Equals(OUR_WALLET, StringComparison.OrdinalIgnoreCase); // El dinero se ingresa en nuestra cuenta
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al esperar la transacción: {ex.Message}");
            }

            transaction.Completed = success;
            await _dbContext.SaveChangesAsync();
            return success;
        }
    }
}
