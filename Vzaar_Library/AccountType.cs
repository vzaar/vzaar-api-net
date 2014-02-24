using System;

namespace VzaarAPI
{
    /// <summary>
    /// This class represents an account type. Vzaar's membership allows a different types of accounts to be created
    /// as part of their subscription packages.
    /// </summary>
    public class AccountType
    {        
        #region Private Members     

        private double _version;
        private int _accountId;
        private string _title;
        private int _monthly;
        private string _currency;
        private long _bandwidth;
        private bool _borderless;
        private bool _searchEnhancer;

        #endregion
        
        #region Public and Package Protected Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="version">The vzaar API version number</param>
        /// <param name="accountId">The vzaar account ID</param>
        /// <param name="title">The name of the vzaar account</param>
        /// <param name="monthly">The monthly cost of the account in the given currency</param>
        /// <param name="currency">The currency the account is charged in. Currently this is only in dollars</param>
        /// <param name="bandwidth">The amount of monthly bandwidth allocated to a user for video service and playing</param>
        /// <param name="borderless">If the user is allowed to use a player with no skin</param>
        /// <param name="searchEnhancer">If the user is allowed to optimize where google directs video traffic</param>
        public AccountType(double version,
            int accountId,
            string title,
            int monthly,
            string currency,
            long bandwidth,
            bool borderless,
            bool searchEnhancer)
        {
            _version = version;
            _accountId = accountId;
            _title = title;
            _monthly = monthly;
            _currency = currency;
            _bandwidth = bandwidth;
            _borderless = borderless;
            _searchEnhancer = searchEnhancer;
        }

        /// <summary>
        /// The vzaar API version number.
        /// </summary>
        /// <returns>The vzaar API version number</returns>
        public double GetVersion()
        {
            return _version;
        }

        /// <summary>
        /// The vzaar account ID.
        /// </summary>
        /// <returns>The vzaar account ID</returns>
        public int GetAccountId()
        {
            return _accountId;
        }

        /// <summary>
        /// The name of the vzaar account.
        /// </summary>
        /// <returns>The name of the vzaar account</returns>
        public string GetTitle()
        {
            return _title;
        }

        /// <summary>
        /// The monthly cost of the account in the given currency.
        /// </summary>
        /// <returns>The monthly cost of the account in the given currency</returns>
        public int GetMonthly()
        {
            return _monthly;
        }


        /// <summary>
        /// The currency the account is charged in. Currently this is only in dollars.
        /// </summary>
        /// <returns>The currency the account is charged in</returns>
        public string GetCurrency()
        {
            return _currency;
        }

        /// <summary>
        /// The amount of monthly bandwidth allocated to a user for video service 
        /// and playing.
        /// </summary>
        /// <returns>The amount of monthly bandwidth allocated</returns>
        public long GetBandwidth()
        {
            return _bandwidth;
        }


        /// <summary>
        /// Is the user is allowed to use a player with no skin.
        /// </summary>
        /// <returns>The user is allowed to use a player with no skin</returns>
        public bool IsBorderless()
        {
            return _borderless;
        }


        /// <summary>
        /// Is the user is allowed to optimize where google directs video traffic.
        /// </summary>
        /// <returns>The user is allowed to optimize where google directs video traffic</returns>
        public bool IsSearchEnhancer()
        {
            return _searchEnhancer;
        }

        /// <summary>
        /// String representation of the contents of this account bean.
        /// </summary>
        /// <returns>String representation of the contents of this account bean.</returns>
        public override string ToString()
        {
            return String.Format("version={0}, account={1}, title={2}, monthly={3}, currency={4}, bandwidth={5},  searchEnhancer={6}",
                _version, _accountId, _title, _monthly, _currency, _bandwidth, _searchEnhancer);
        }

        #endregion
    }
}
