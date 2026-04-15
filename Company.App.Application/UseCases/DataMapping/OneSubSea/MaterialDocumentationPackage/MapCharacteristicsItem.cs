using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.MaterialDocumentationPackage
{
    public class MapCharacteristicsItem : ICharacteristicsItemMapper
    {
        public CharacteristicsItem Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var words = document.Words;

            var consequenceOfFailure =

            var createdBy =
            
            var criticalRating =
            
            var dataAdditional =
            
            var division =
            
            var internalDiameter =
            
            var labDesignOffice =
            
            var length =
            
            var mirid =
            
            var manufacturer =
            
            var manufacturerPartNumber =
            
            var material =
            
            var materialGroup =
            
            var matlBasic =
            
            var outsideDiameter =
            
            var polymerSealType =
            
            var probabilityOfFailure =
            
            var sdrLandQM =
            
            var standardMaterial =
            
            var traceabilityCode =
            
            var xPlantStatus =

            return new CharacteristicsItem(
                consequenceOfFailure,
                createdBy,
                criticalRating,
                dataAdditional,
                division,
                internalDiameter,
                labDesignOffice,
                length,
                mirid,
                manufacturer,
                manufacturerPartNumber,
                material,
                materialGroup,
                matlBasic,
                outsideDiameter,
                polymerSealType,
                probabilityOfFailure,
                sdrLandQM,
                standardMaterial,
                traceabilityCode,
                xPlantStatus);
        }
    }
}
