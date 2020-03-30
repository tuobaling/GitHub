using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.BLL.Repositories;
using Trello.DAL.Models;

namespace Trello.BLL.Services
{
    public interface ITicketsService
    {
        Task<IEnumerable<Ticket>> Get(int id, string ownerId);
        int Create(Ticket item);
        int Update(Ticket item);
        int Delete(int ticketId);

        HttpStatusCode UpdatePosition(List<Ticket> data);
    }
    public class TicketsService : ITicketsService
    {
        private readonly ITicketsRepository _repository;

        public TicketsService(ITicketsRepository repository)
        {
            _repository = repository;
        }
        public int Create(Ticket item)
        {
            return _repository.Create(item);
        }

        public int Delete(int ticketId)
        {
            return _repository.Delete(ticketId);
        }

        public Task<IEnumerable<Ticket>> Get(int id, string ownerId)
        {
            throw new NotImplementedException();
        }

        public int Update(Ticket item)
        {
            return _repository.Update(item);
        }

        public HttpStatusCode UpdatePosition(List<Ticket> data)
        {
            return _repository.UpdatePosition(data);
        }
    }
}
