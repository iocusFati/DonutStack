using System;
using GridFolder.GridsHolderFolder;

namespace Infrastructure.Services.Factories
{
    public interface ILocationFactory : IService
    {
        void CreateLocation();
        event Action<GridsHolder> OnLocationCreated;
    }
}