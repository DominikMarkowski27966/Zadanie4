using System;

namespace LegacyApp
{
    public interface IClientRepository
    {
        Client GetById(int idClient);
    }

    public interface ICreditLimitService
    {
        int GetCreditLimit(String lastName, DateTime birthdate);
    }

    public interface IUserValidationService
    {
        bool ValidateUser(string firstName, string lastName, string email, DateTime birthdate);
    }

    public class UserService
    {
        private IClientRepository _clientRepository;
        private ICreditLimitService _creditLimitService;
        private IUserValidationService _userValidationService;

        public UserService(IClientRepository clientRepository, ICreditLimitService creditLimitService,
            IUserValidationService userValidationService)
        {
            _clientRepository = clientRepository;
            _creditLimitService = creditLimitService;
            _userValidationService = userValidationService;
        }

        [Obsolete]
        public UserService()
        {
            // _clientRepository = new ClientRepository();
            // _creditLimitService = new UserCreditService();
            // _userValidationService = new ClientRepository();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!_userValidationService.ValidateUser(firstName, lastName, email, dateOfBirth))
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                user.CreditLimit = _creditLimitService.GetCreditLimit(lastName, dateOfBirth);
            }
            else
            {
                user.HasCreditLimit = true;
                user.CreditLimit = _creditLimitService.GetCreditLimit(lastName, dateOfBirth);
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
    }
}