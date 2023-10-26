using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Addresses.AppLogic.Convertors
{
    /// <summary>
    /// Converts address objects
    /// </summary>
    public interface IAddressConvertor
    {

        // clones address
        Address CloneAddress(Address address, bool forRegistration);

        // converts form address to db address
        Address ConvertToAddress(AddressForm addressForm, bool forRegistration);

        // converts incomplete form address to db address
        Address ConvertToAddress(IncompleteAddressForm addressForm, bool forRegistration);

        // converts db address to form address
        AddressForm ConvertToAddressForm(Address address);

        // converts db address to incomplete  form address
        IncompleteAddressForm ConvertToIncompleteAddressForm(Address address);

        // converts db address to address detail
        AddressDetail ConvertToAddressDetail(Address address);

        // updates db address
        void UpdateAddresss(Address toUpdate, Address newAddress);

    }

}
