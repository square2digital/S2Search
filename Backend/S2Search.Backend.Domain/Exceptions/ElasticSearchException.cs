using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S2Search.Backend.Domain.Exceptions
{
    public class ElasticSearchException : Exception
    {
        public ElasticSearchException()
        {

        }

        public ElasticSearchException(string message) : base(message)
        {

        }

        public ElasticSearchException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
