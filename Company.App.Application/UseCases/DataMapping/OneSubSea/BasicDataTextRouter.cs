using Company.App.Application.Interfaces;
using Company.App.Application.Interfaces.OneSubSea.SharedMappers;
using Company.App.Application.UseCases.DataMapping.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea
{
    internal class BasicDataTextRouter : IBasicDataTextRouter
    {
        private readonly IEnumerable<IBasicDataTextMapper> _mappers;

        public BasicDataTextRouter(IEnumerable<IBasicDataTextMapper> mappers)
        {
            _mappers = mappers;
        }

        public IBasicDataTextMapper Resolve(DocumentType documentType)
        {
            var mapper = _mappers.FirstOrDefault(m => m.SupportedType == documentType);

            if (mapper is null)
            {
                throw new InvalidOperationException(
                    $"No BasicDataText mapper registered for document type '{documentType}'.");
            }

            return mapper;
        }
    }
}
