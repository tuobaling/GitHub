using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.DAL.Repositories;
using Trello.DAL.Models;
using Trello.DAL.UnitOfWorks;

namespace Trello.BLL.Services
{
    public interface ITicketsService
    {
        IEnumerable<Ticket> Get();
        IQueryable<Ticket> Query();
        Ticket GetById(int id);
        Ticket GetFirstOrDefault();
        void Insert(Ticket data);

        void Update(Ticket data);

        void Delete(int id);
        void Delete(Ticket data);
        void UpdatePosition(IEnumerable<Ticket> data);
    }
    public class TicketsService : ITicketsService
    {
        private readonly IUnitOfWorks _unitOfWork;
        public TicketsService(IUnitOfWorks unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<Ticket>().Delete(id);
            _unitOfWork.Save();
        }

        public void Delete(Ticket data)
        {
            _unitOfWork.Repository<Ticket>().Delete(data);
            _unitOfWork.Save();
        }

        public IEnumerable<Ticket> Get()
        {
            return _unitOfWork.Repository<Ticket>().Get().ToList();
        }

        public Ticket GetById(int id)
        {
            return _unitOfWork.Repository<Ticket>().Get(data => data.Id == id).FirstOrDefault();
        }

        public Ticket GetFirstOrDefault()
        {
            return _unitOfWork.Repository<Ticket>().Get().FirstOrDefault();
        }

        public void Insert(Ticket data)
        {
            _unitOfWork.Repository<Ticket>().Insert(data);
            _unitOfWork.Save();
        }

        public IQueryable<Ticket> Query()
        {
            throw new NotImplementedException();
        }

        public void Update(Ticket data)
        {
            _unitOfWork.Repository<Ticket>().Update(data);
            _unitOfWork.Save();
        }

        public void UpdatePosition(IEnumerable<Ticket> data)
        {
            var boardId = data.ElementAt(0).BoardId;

            foreach (Ticket t in data)
            {
                var ticket = _unitOfWork.Repository<Ticket>().Get((i) => i.Id == t.Id && i.BoardId == boardId).FirstOrDefault();
                ticket.PositionNo = t.PositionNo;
                _unitOfWork.Repository<Ticket>().Update(ticket);
            }
            _unitOfWork.Save();
        }
    }
}
