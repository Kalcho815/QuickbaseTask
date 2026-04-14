using Backend.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend;

public class ConcreteStatService : IStatService
{
    public ConcreteStatService()
    {
            
    }

    public IEnumerable<CountryPopulationDTO>? GetCountryPopulations()
    {
        //returning objects instead of a dictionary because data is usually automatically casted when deserialised from an api response

        return new List<CountryPopulationDTO>
        {
            new CountryPopulationDTO { CountryName = "India", Population = 1182105000 },
            new CountryPopulationDTO { CountryName = "United Kingdom", Population = 62026962 },
            new CountryPopulationDTO { CountryName = "Chile", Population = 17094270 },
            new CountryPopulationDTO { CountryName = "Mali", Population = 15370000 },
            new CountryPopulationDTO { CountryName = "Greece", Population = 11305118 },
            new CountryPopulationDTO { CountryName = "Armenia", Population = 3249482 },
            new CountryPopulationDTO { CountryName = "Slovenia", Population = 2046976 },
            new CountryPopulationDTO { CountryName = "Saint Vincent and the Grenadines", Population = 109284 },
            new CountryPopulationDTO { CountryName = "Bhutan", Population = 695822 },
            new CountryPopulationDTO { CountryName = "Aruba (Netherlands)", Population = 101484 },
            new CountryPopulationDTO { CountryName = "Maldives", Population = 319738 },
            new CountryPopulationDTO { CountryName = "Mayotte (France)", Population = 202000 },
            new CountryPopulationDTO { CountryName = "Vietnam", Population = 86932500 },
            new CountryPopulationDTO { CountryName = "Germany", Population = 81802257 },
            new CountryPopulationDTO { CountryName = "Botswana", Population = 2029307 },
            new CountryPopulationDTO { CountryName = "Togo", Population = 6191155 },
            new CountryPopulationDTO { CountryName = "Luxembourg", Population = 502066 },
            new CountryPopulationDTO { CountryName = "U.S. Virgin Islands (US)", Population = 106267 },
            new CountryPopulationDTO { CountryName = "Belarus", Population = 9480178 },
            new CountryPopulationDTO { CountryName = "Myanmar", Population = 59780000 },
            new CountryPopulationDTO { CountryName = "Mauritania", Population = 3217383 },
            new CountryPopulationDTO { CountryName = "Malaysia", Population = 28334135 },
            new CountryPopulationDTO { CountryName = "Dominican Republic", Population = 9884371 },
            new CountryPopulationDTO { CountryName = "New Caledonia (France)", Population = 248000 },
            new CountryPopulationDTO { CountryName = "Slovakia", Population = 5424925 },
            new CountryPopulationDTO { CountryName = "Kyrgyzstan", Population = 5418300 },
            new CountryPopulationDTO { CountryName = "Lithuania", Population = 3329039 },
            new CountryPopulationDTO { CountryName = "United States of America", Population = 309349689 }
        };
    }


    public async Task<IEnumerable<CountryPopulationDTO>?> GetCountryPopulationsAsync()
    {
        return await Task.FromResult(GetCountryPopulations());
    }
}
