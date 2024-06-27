using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.CQRS
{
    public interface IQueryHandler<in TQuery,Tresponse>
        :IRequestHandler<TQuery,Tresponse>
        where TQuery : IQuery<Tresponse>
        where Tresponse : notnull
    {

    }

    

}
