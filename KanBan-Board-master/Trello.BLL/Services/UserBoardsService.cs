using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.BLL.Repositories;
using Trello.DAL.Models;

namespace Trello.BLL.Services
{
    public interface IUserBoardsService
    {
        Task<IEnumerable<UserBoard>> Get(int id, string ownerId);
        int Create(UserBoard item);
        void Update(UserBoard item);
        int Delete(int id, out int boardId);
    }
    public class UserBoardsService : IUserBoardsService
    {
        private readonly IUserBoardsRepository _repository;

        public UserBoardsService(IUserBoardsRepository repository)
        {
            _repository = repository;
        }

        //// GET: UserBoards
        public Task<IEnumerable<UserBoard>> Get(int id, string ownerId)
        {
            return _repository.GetAsync(id, ownerId);
        }

        public int Create(UserBoard userBoard)
        {
            return _repository.Create(userBoard);
        }

        public void Update(UserBoard userBoard)
        {
            _repository.Update(userBoard);
        }

        public int Delete(int id, out int boardId)
        {
            return _repository.Delete(id, out boardId);
        }
    }
}
