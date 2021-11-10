using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Logic.DTO
{
    public class Cart : IEnumerable<CartItem>
    {

        private readonly List<CartItem> items = new List<CartItem>();

        public Guid Id { get; set; }

        public IEnumerator<CartItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }

        public int Count => items.Count;

        public decimal Total => items.Sum(a => a.Amount);

        public void Add(CartItem item)
        {
            items.Add(item);
        }

        public void AddRange(IEnumerable<CartItem> collection)
        {
            items.AddRange(collection);
        }
    }
}
