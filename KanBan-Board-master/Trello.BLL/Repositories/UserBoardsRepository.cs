using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.DAL.Models;

namespace Trello.BLL.Repositories
{
    public interface IUserBoardsRepository
    {
        Task<IEnumerable<UserBoard>> GetAsync(int id, string ownerId);
        int Create(UserBoard item);
        void Update(UserBoard item);
        int Delete(int id, out int boardId);
    }
    public class UserBoardsRepository : IUserBoardsRepository
    {
        private int RetunMsg_Success = 1;
        private int RetunMsg_Failed = 0;
        private DataModel db = new DataModel();

        public async Task<IEnumerable<UserBoard>> GetAsync(int id, string ownerId)
        {
            //***   ownerId 不知道要怎麼處理
            ownerId = db.Boards.First((i) => i.Id == id).UserId;

            Task<IQueryable<UserBoard>> userBoards = Task.Factory.StartNew(
               () => db.UserBoards.Where((i) => i.BoardId == id).Include(u => u.AspNetUser).Include(u => u.Board)
            );

            await Task.WhenAll(userBoards);

            IEnumerable<UserBoard> UserBoards = userBoards.Result.ToList();

            return UserBoards;
        }

        public int Create(UserBoard userBoard)
        {
            userBoard.Id = new Random().Next();
            try
            {
                db.UserBoards.Add(userBoard);
                db.SaveChanges();
                return RetunMsg_Success;
            }
            catch
            {
                return RetunMsg_Failed;
            }
        }

        public void Update(UserBoard item)
        {
            throw new NotImplementedException();
        }

        public int Delete(int id, out int boardId)
        {
            UserBoard userBoard = db.UserBoards.Find(id);
            boardId = userBoard.BoardId;
            try
            {
                db.UserBoards.Remove(userBoard);
                db.SaveChanges();
                return RetunMsg_Success;
            }
            catch
            {
                return RetunMsg_Failed;
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    //base.Dispose(disposing);
        //}
    }
}
