using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public interface IOperationRepository
    {
        IQueryable<Operation> GetAllOperations();
        
        Operation GetOperationById(int? id);


        void AddOperation(Operation operation);

        void UpdateOperation(Operation operation);

        void DeleteOperation(Operation operation);


    }
}
