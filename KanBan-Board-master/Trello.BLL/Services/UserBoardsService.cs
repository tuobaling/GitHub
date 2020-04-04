using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trello.DAL.Models;
using Trello.DAL.UnitOfWorks;

namespace Trello.BLL.Services
{
    public interface IUserBoardsService
    {
        int GetBoardIdById(int id);
        string GetBoardUserIdById(int id);
        UserBoard GetUserBoard(int id);
        Task<IEnumerable<UserBoard>> GetUserBoardsAsync(int id);
        void Delete(UserBoard data);
        void Dispose(bool disposing);
        void Insert(UserBoard data);

    }
    public class UserBoardsService : IUserBoardsService
    {
        public IUnitOfWorks _unitOfWork { get; set; }
        public UserBoardsService(IUnitOfWorks unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int GetBoardIdById(int id)
        {
            return _unitOfWork.Repository<UserBoard>().Get(id).BoardId;
        }
        public string GetBoardUserIdById(int id)
        {
            return _unitOfWork.Repository<Board>().Get(id).UserId;
        }

        public UserBoard GetUserBoard(int id)
        {
            return _unitOfWork.Repository<UserBoard>().Get((i) => i.BoardId == id, null, a => a.AspNetUser, b => b.Board).SingleOrDefault();
        }
        public async Task<IEnumerable<UserBoard>> GetUserBoardsAsync(int id)
        {
            return await _unitOfWork.Repository<UserBoard>().GetAsync((i) => i.BoardId == id, null, a => a.AspNetUser, b => b.Board);
        }
        public void Delete(UserBoard data)
        {
            _unitOfWork.Repository<UserBoard>().Delete(data);
            _unitOfWork.SaveChanges();
        }
        public void Dispose(bool disposing)
        {
            if (disposing)
                _unitOfWork.Dispose();
        }
     

        //public void Delete(UserBoard data)
        //{
        //    _unitOfWork.Repository<UserBoard>().Delete(data);
        //    _unitOfWork.Save();
        //}

        //public IEnumerable<UserBoard> Get()
        //{
        //    return _unitOfWork.Repository<UserBoard>().Get().ToList();
        //}

        //public IEnumerable<UserBoard> Get(int id)
        //{
        //    return _unitOfWork.Repository<UserBoard>().Get((i) => i.BoardId == id, null , a => a.AspNetUser, b => b.Board).ToList();
        //}

        //public UserBoard GetById(int id)
        //{
        //    return _unitOfWork.Repository<UserBoard>().Get(data => data.Id == id).FirstOrDefault();
        //}
        //public Board GetBoardById(int id)
        //{
        //    return _unitOfWork.Repository<Board>().Get(data => data.Id == id).FirstOrDefault();
        //}

        //public UserBoard GetFirstOrDefault()
        //{
        //    return _unitOfWork.Repository<UserBoard>().Get().FirstOrDefault();
        //}

        public void Insert(UserBoard data)
        {
            data.Id = new Random().Next();

            _unitOfWork.Repository<UserBoard>().Insert(data);
            _unitOfWork.SaveChanges();
        }

        //public IQueryable<UserBoard> Query()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update(UserBoard data)
        //{
        //    _unitOfWork.Repository<UserBoard>().Update(data);
        //    _unitOfWork.Save();
        //}
    }
}
