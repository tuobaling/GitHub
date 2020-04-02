using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.DAL.Repositories;
using Trello.DAL.Models;
using Trello.DAL.UnitOfWorks;

namespace Trello.BLL.Services
{
    public interface IUserBoardsService
    {
        IEnumerable<UserBoard> Get();
        IEnumerable<UserBoard> Get(int id);
        IQueryable<UserBoard> Query();
        UserBoard GetById(int id);
        Board GetBoardById(int id);
        UserBoard GetFirstOrDefault();
        void Insert(UserBoard data);

        void Update(UserBoard data);

        void Delete(int id);
        void Delete(UserBoard data);
    }
    public class UserBoardsService : IUserBoardsService
    {
        private readonly IUnitOfWorks _unitOfWork;
        public UserBoardsService(IUnitOfWorks unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Delete(int id)
        {
            _unitOfWork.Repository<UserBoard>().Delete(id);
            _unitOfWork.Save();
        }

        public void Delete(UserBoard data)
        {
            _unitOfWork.Repository<UserBoard>().Delete(data);
            _unitOfWork.Save();
        }

        public IEnumerable<UserBoard> Get()
        {
            return _unitOfWork.Repository<UserBoard>().Get().ToList();
        }

        public IEnumerable<UserBoard> Get(int id)
        {
            return _unitOfWork.Repository<UserBoard>().Get((i) => i.BoardId == id, null , a => a.AspNetUser, b => b.Board).ToList();
        }

        public UserBoard GetById(int id)
        {
            return _unitOfWork.Repository<UserBoard>().Get(data => data.Id == id).FirstOrDefault();
        }
        public Board GetBoardById(int id)
        {
            return _unitOfWork.Repository<Board>().Get(data => data.Id == id).FirstOrDefault();
        }

        public UserBoard GetFirstOrDefault()
        {
            return _unitOfWork.Repository<UserBoard>().Get().FirstOrDefault();
        }

        public void Insert(UserBoard data)
        {
            data.Id = new Random().Next();

            _unitOfWork.Repository<UserBoard>().Insert(data);
            _unitOfWork.Save();
        }

        public IQueryable<UserBoard> Query()
        {
            throw new NotImplementedException();
        }

        public void Update(UserBoard data)
        {
            _unitOfWork.Repository<UserBoard>().Update(data);
            _unitOfWork.Save();
        }
    }
}
