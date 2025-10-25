# KutokAccounting

A streamlined and focused accounting application designed to help small businesses automate vendor management, track daily financials, and calculate profitability with ease.

---

## Key Features

- **Independent Store Management:** сreate and manage multiple stores, each acting as a self-contained accounting entity. Every store has its own profile with a name, address, opening date, and operational status.
    
- **Segregated Multi-Store Accounting:** all financial records, including transactions and invoices, are strictly tied to a single store. This ensures clean, isolated bookkeeping for each business location.
    
- **Independent Store Analytics:** select any accounting store page to see, manage its transactions and accurately calculate its profitability without interference from other stores.
    
- **Centralized Vendor Management:** manage a single, shared list of vendors for your entire business. Each vendor profile can be created with a name and a detailed description, providing a unified database of suppliers accessible from any store.
  
- **Customizable Transaction Types:** define custom categories for your financial operations. Each type is assigned a name and a sign (`+` for income or `-` for expense), which automatically classifies the transaction and ensures accurate profit calculation.
    
- **Powerful Financial Reporting:** instantly calculate net profit for any store over a selected date range. Drill down into your finances by filtering transactions by a specific vendor to understand your costs and revenue streams with greater clarity.
## Technology Stack & Architecture

This application is built with a monolithic clean architecture on the .NET platform, ensuring simple maintenance and deployment. The technology stack was chosen for robustness and a modern development experience.

- **Framework:** **.NET MAUI Blazor Hybrid**

- **Database:** **SQLite**
    
- **Data Access:** **Entity Framework Core**
    
- **Validation:** **FluentValidation** 
    
- **Logging:** **Custom File Logger** 
    
- **Testing:** **xUnit** 
