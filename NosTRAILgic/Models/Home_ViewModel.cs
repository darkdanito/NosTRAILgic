using System.Collections.Generic;

namespace NosTRAILgic.Models
{
    /************************************************************************************
     * Description: This ViewModel manages the handling of display of                   *
     *              Home/Index                                                          *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 24/03/2016                                                                 *
     ************************************************************************************/
    public class Home_ViewModel
    {
        public IEnumerable<Location> enumerableAllLocation { get; set; }                // Contains the Enumerable of the Locaions

        public IEnumerable<Weather> enumerableAllWeather { get; set; }                  // Contains the Enumerable of the Weather
    }
}