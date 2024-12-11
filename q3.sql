WITH DepartmentAverage AS (
    SELECT 
        d.id AS department_id,
        COALESCE(AVG(e.salary), 0) AS department_avg_salary -- Calculate department average salary, handle no employees
    FROM 
        departments d
    LEFT JOIN 
        employees e ON d.id = e.departmentId
    GROUP BY 
        d.id
),
CompanyAverage AS (
    SELECT 
        AVG(salary) AS company_avg_salary -- Calculate company's overall average salary
    FROM 
        employees
)
SELECT 
    da.department_id,
    da.department_avg_salary,
    CASE 
        WHEN da.department_avg_salary > ca.company_avg_salary THEN 'Above' -- Compare department's avg with company's avg
        WHEN da.department_avg_salary < ca.company_avg_salary THEN 'Below'
        ELSE 'Equal'
    END AS status
FROM 
    DepartmentAverage da
CROSS JOIN 
    CompanyAverage ca; -- Cross join to use the company's average for comparison