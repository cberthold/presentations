using App.Web1.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.Web1.Controllers
{
    [Authorize]
    public class RandomSaleController : ApiController
    {
        // GET api/randomsale/random
        [Authorize]
        [ActionName("random")]
        public ResponsePayload<IEnumerable<RandomSale>> GetRandomSale()
        {
            // get our repository
            var repository = RandomUserRepository.GetInstance();
            var salesRepo = RandomSaleRepository.GetInstance();
            // get the list of users from the repository
            var users = repository.GetUsers();
            // use Fisher yates shuffle to get random sale -
            // http://en.wikipedia.org/wiki/Fisher-Yates_shuffle#The_modern_algorithm
            User randomUser = null;

            // get a random user
            randomUser = users.Shuffle().FirstOrDefault();

            var location = randomUser.Location;



            var now = DateTime.Now;
            var randInt = now.Millisecond + (now.Second * 1000) + (now.Minute * 60 * 1000) + (now.Hour * 3600 * 1000);
            // generate random amount
            var rand = new Random(randInt);
            // make it in the thousands to be impressive
            var salesAmount = rand.Next(3, 500) * 1000;

            var sale = new RandomSale()
            {
                Amount = salesAmount,
                CreatedBy = this.RequestContext.Principal.Identity.Name,
                CreatedDate = DateTime.Now,
                User = randomUser
            };

            // check the zip exists on the sale for display
            repository.CheckUserLocation(sale);

            // add the sale to the repo
            salesRepo.AddRandomSale(sale);

            return GetAllSales();
        }

        // GET api/randomsale/all
        [Authorize]
        [ActionName("all")]
        public ResponsePayload<IEnumerable<RandomSale>> GetAllSales()
        {
            ISpecification<RandomSale> spec;
            string user = RequestContext.Principal.Identity.Name;

            // get our repository
            var salesRepo = RandomSaleRepository.GetInstance();

            if (user == "Milton")
                spec = new TrueSpecification<RandomSale>();
            else
                spec = new ExpressionSpecification<RandomSale>(sale => sale.CreatedBy == user);

            var sales = from s in salesRepo.GetAllSales()
                        where spec.IsSatisfiedBy(s)
                        orderby s.CreatedDate descending
                        select s;

            var payload = new ResponsePayload<IEnumerable<RandomSale>>();
            payload.Payload = sales;

            return payload;
        }

    }
}
