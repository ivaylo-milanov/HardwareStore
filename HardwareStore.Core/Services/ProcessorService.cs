namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Processor;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProcessorService : IProcessorService
    {
        private readonly IRepository repository;

        public ProcessorService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ProcessorViewModel>> GetAllProducts()
            => await this.repository.AllReadonly<Product>(p => p.Category.Name == "Processor")
                .Select(p => new ProcessorViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    AddDate = p.AddDate,
                    Manufacturer = p.Manufacturer!.Name,
                    Series = p.ProductAttributes.GetAttributeValue(nameof(ProcessorViewModel.Series)),
                    Generation = p.ProductAttributes.GetAttributeValue(nameof(ProcessorViewModel.Generation)),
                    Socket = p.ProductAttributes.GetAttributeValue(nameof(ProcessorViewModel.Socket)),
                    BoxCooler = p.ProductAttributes.GetAttributeValue(nameof(ProcessorViewModel.BoxCooler))
                })
                .ToListAsync();
    }
}
