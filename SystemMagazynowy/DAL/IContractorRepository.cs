using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public interface IContractorRepository
    {
        IQueryable<Contractor> getAllContractors();

        Contractor GetContractorById(int? id);

        void AddContractor(Contractor contractor);

        void DeleteContractor(Contractor contractor);

        void UpdateContractor(Contractor contractor);

    }
}
