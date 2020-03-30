using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.DAL.Models;

namespace Trello.BLL.Repositories
{
    public interface ITicketsRepository
    {
        Task<IEnumerable<Ticket>> Get(int id, string ownerId);
        int Create(Ticket item);
        int Update(Ticket item);
        int Delete(int ticketId);

        HttpStatusCode UpdatePosition(List<Ticket> data);
    }
    public class TicketsRepository : ITicketsRepository
    {
        private int RetunMsg_Success = 1;
        private int RetunMsg_Failed = 0;
        private DataModel db = new DataModel();

        public int Create(Ticket item)
        {
            int latestPoistion = (from c in db.Tickets
                                  where c.BoardId == item.BoardId
                                  orderby c.PositionNo descending
                                  select c.PositionNo).FirstOrDefault();


            item.CreatedOn = DateTime.Now;
            item.PositionNo = latestPoistion + 1;
            db.Tickets.Add(item);
            try
            {
                db.SaveChanges();
                return RetunMsg_Success;
            }
            catch
            {
                return RetunMsg_Failed;
            }
        }

        public int Delete(int ticketId)
        {
            try
            {
                Ticket tickets = db.Tickets.Find(ticketId);

                tickets.Cards.ToList().ForEach((i) =>
                {
                    db.TaskItems.RemoveRange(i.TaskItems);
                });

                db.Cards.RemoveRange(tickets.Cards);
                db.Tickets.Remove(tickets);
                db.SaveChanges();
                return RetunMsg_Success;
            }
            catch
            {
                return RetunMsg_Failed;
            }
        }

        public Task<IEnumerable<Ticket>> Get(int id, string ownerId)
        {
            throw new NotImplementedException();
        }

        public int Update(Ticket item)
        {

            var tickets = db.Tickets.FirstOrDefault(i => i.Id == item.Id);
            tickets.Name = item.Name;

            db.Entry(tickets).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
                return RetunMsg_Success;
            }
            catch
            {
                return RetunMsg_Failed;
            }
            //***   不知道要幹嘛
            //ViewBag.BoardId = new SelectList(db.Boards, "Id", "Name", ticket.BoardId);
        }

        public HttpStatusCode UpdatePosition(List<Ticket> data)
        {
            var boardId = data[0].BoardId;
            var tickets = db.Tickets.Where((i) => i.BoardId == boardId).ToList();

            foreach (Ticket t in data)
            {
                var ticket = tickets.First((i) => i.Id == t.Id);
                ticket.PositionNo = t.PositionNo;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
            }
                return HttpStatusCode.OK;
        }
    }
}
