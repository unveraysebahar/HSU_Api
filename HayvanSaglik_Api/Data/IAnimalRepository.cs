using HayvanSaglik_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public interface IAnimalRepository
    {
        List<Animal> GetAnimals();
        Animal GetAnimalById(int animalId);
        Animal DeleteAnimalById(int animalId);
        List<HealthInformation> GetHealthInformationsByAnimal(int animalId);
        Task<Animal> GetAnimal(int animalId);
        Task<Animal> UpdateAnimal(Animal animal);
    }
}
