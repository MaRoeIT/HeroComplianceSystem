using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents additional basic data text values associated with a material description and items in Purchase Order.
    /// </summary>
    public record BasicDataText
    {
        // Gets the delivery requirement expiry date text.
        public string DeliveryRequirementExpiryDate { get; }

        // Gets the Seal SQ description.
        public string SealSQDescription { get; }

        // Gets the Seal Engineering part number.
        public string SealEngineeringPartNumber { get; }

        // Gets the T-seal value.
        public string TSeal { get; }

        // Gets the anti-extrusion rings value.
        public string AntiExtrusionRings { get; }

        // Constructor.
        public BasicDataText(
            string deliveryRequirementExpiryDate,
            string sealSQDescription,
            string sealEngineeringPartNumber,
            string tSeal,
            string antiExtrusionRings)
        {
            DeliveryRequirementExpiryDate = deliveryRequirementExpiryDate;
            SealSQDescription = sealSQDescription;
            SealEngineeringPartNumber = sealEngineeringPartNumber;
            TSeal = tSeal;
            AntiExtrusionRings = antiExtrusionRings;
        }
    }
}
