using HayvanSaglik_Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public class AnimalRepository : IAnimalRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public AnimalRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Animal DeleteAnimalById(int animalId)
        {
            var animal = _applicationDbContext.Animals.Include(a => a.HealthInformations).FirstOrDefault(a => a.Id == animalId);
            _applicationDbContext.Remove(animal);
            return animal;
        }

        public async Task<Animal> GetAnimal(int animalId)
        {
            return await _applicationDbContext.Animals.FirstOrDefaultAsync(x => x.Id == animalId);
        }

        public Animal GetAnimalById(int animalId)
        {
            var animal = _applicationDbContext.Animals.Include(a => a.HealthInformations).FirstOrDefault(a => a.Id == animalId);
            return animal;
        }

        public List<Animal> GetAnimals()
        {
            var animals = _applicationDbContext.Animals.Include(a => a.HealthInformations).ToList();
            return animals;
        }

        public List<HealthInformation> GetHealthInformationsByAnimal(int animalId)
        {
            var healthInformations = _applicationDbContext.HealthInformations.Where(h => h.AnimalId == animalId).ToList();
            return healthInformations;
        }

        public async Task<Animal> UpdateAnimal(Animal animal)
        {
            var result = await _applicationDbContext.Animals.FirstOrDefaultAsync(x => x.Id == animal.Id);

            if (result != null)
            {
                result.Name = animal.Name;
                result.IsNeutered = animal.IsNeutered;

                await _applicationDbContext.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
