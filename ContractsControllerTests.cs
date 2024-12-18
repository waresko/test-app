using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication2.Entities;
using WebApplication2.Models;

namespace WebApplication2;

public class ContractsControllerShould
{
    readonly ApplicationDbContext _dbcontext;
    readonly ContractsController _controller;
    public ContractsControllerShould()
    {
        _dbcontext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb").Options);
       if(_dbcontext.ProductionFacilities.Count() <= 0)
        {
            _dbcontext.ProductionFacilities.Add(new ProductionFacility { Code = "PF002", Name = "Facility2", Area = 600 });
            _dbcontext.ProcessEquipmentTypes.Add(new ProcessEquipmentType { Code = "EQ002", Name = "Type2", Area = 60 });
            _dbcontext.SaveChanges();
        }
        _controller = new ContractsController(_dbcontext);
    }
    [Fact]
    public void GetContracts_ReturnsContractsList()
    {
        _dbcontext.EquipmentPlacementContracts.Add(new EquipmentPlacementContract
        {
            ProductionFacilityCode = "PF002",
            ProcessEquipmentTypeCode = "EQ002",
            EquipmentUnits = 5,
        });
        _dbcontext.SaveChanges();

        var result = _controller.GetContracts();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var contracts = Assert.IsAssignableFrom<IEnumerable<EquipmentContract>>(okResult.Value);
        Assert.Single(contracts);
        var contract = contracts.First();
        Assert.Equal("Facility2", contract.ProductionFacilityName);
        Assert.Equal("Type2", contract.EquipmentTypeName);
        Assert.Equal(5, contract.EquipmentUnits); 
    }
}
