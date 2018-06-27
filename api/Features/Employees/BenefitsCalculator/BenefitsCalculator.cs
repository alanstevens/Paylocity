﻿using System.Collections.Generic;
using System.Linq;
using Paylocity.API.Shared.Entities;

namespace Paylocity.API.Features.Employees.BenefitsCalculator
{
    public class BenefitsCalculator
    {
        // TODO: I'm uncomfortable mutating these values by reference. - HAS 06/25/2018 
        public void CalculateBenefitsCostsFor(Employee employee)
        {
            CalculateBenefitCostForEachDependent(employee.Dependents);

            var totalCostForAllDependents = CalculateAnnualTotalForAllDependents(employee.Dependents);

            employee.PersonalBenefitsCost = CalculateEmployeeBenefitsCost(employee);

            employee.AnnualBenefitsCost = employee.PersonalBenefitsCost + totalCostForAllDependents;

            employee.BenefitsCostPerPaycheck = CalculateBenefitsCostPerPaycheck(employee.AnnualBenefitsCost);
        }

        public void CalculateBenefitCostForEachDependent(IEnumerable<Dependent> dependents)
        {
            dependents?.ToList().ForEach(d => d.PersonalBenefitsCost = CalculateDependentBenefitsCost(d));
        }

        public decimal CalculateAnnualTotalForAllDependents(IEnumerable<Dependent> dependents)
        {
            if (dependents == null || !dependents.Any()) return 0;
            return dependents.Select(d => d.PersonalBenefitsCost).Sum();
        }

        public decimal CalculateEmployeeBenefitsCost(Employee employee)
        {
            var discount = CalculatePersonDiscount(employee);
            return 1000m * discount;
        }

        public decimal CalculateDependentBenefitsCost(Dependent dependent)
        {
            var discount = CalculatePersonDiscount(dependent);
            return 500m * discount;
        }

        public decimal CalculatePersonDiscount(Person person)
        {
            // TODO: Assuming first name here. Requirements are unclear and followup is needed. - HAS 06/24/2018 
            return person.FirstName.StartsWith("A") ? 0.9m : 1m;
        }

        public decimal CalculateBenefitsCostPerPaycheck(decimal annualBenefitsCost) { return annualBenefitsCost / 26m; }
    }
}