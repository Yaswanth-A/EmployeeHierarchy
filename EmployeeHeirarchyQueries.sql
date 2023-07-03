Create DATABASE sampdb;


-- For a Employee Details table
CREATE TABLE EmployeeDetails
(
    EmployeeID uniqueidentifier PRIMARY KEY DEFAULT NEWID(),
    EmployeeName NVARCHAR(50)
    
)
-- Create Node table
CREATE TABLE Employee
(
    employeeID uniqueidentifier
	FOREIGN KEY (EmployeeID) REFERENCES EmployeeDetails(EmployeeID)
) AS Node

--Create Edge table
CREATE TABLE Supplies_to
(
	Designation NVARCHAR(50) NOT NULL
) AS Edge

--Insert values into Employee Details table
INSERT INTO EmployeeDetails([EmployeeName])
VALUES
('Rama'),
('Rajni'),
('Hardhik'),
('Kavana'),
('Yaswanth'),
('Souvik')

-- Insert values into Node table
INSERT INTO Employee
VALUES
((SELECT e.EmployeeID FROM EmployeeDetails e WHERE e.EmployeeName = 'Rama')),
((SELECT e.EmployeeID FROM EmployeeDetails e WHERE e.EmployeeName = 'Rajni')),
((SELECT e.EmployeeID FROM EmployeeDetails e WHERE e.EmployeeName = 'Hardhik')),
((SELECT e.EmployeeID FROM EmployeeDetails e WHERE e.EmployeeName = 'Kavana')),
((SELECT e.EmployeeID FROM EmployeeDetails e WHERE e.EmployeeName = 'Yaswanth')),
((SELECT e.EmployeeID FROM EmployeeDetails e WHERE e.EmployeeName = 'Souvik'));

-- Insert values into edge table
INSERT INTO Supplies_to
VALUES
(
 (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = 'Rajni'),
 (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = 'Rama'),
 ('reports_to')
),
(
 (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = 'Yaswanth'),
 (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = 'Rama'),
 ('reports_to')
),
(
 (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = 'Souvik'),
 (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = 'Rama'),
 ('reports_to')
),
(
 (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = 'Hardhik'),
 (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = 'Rajni'),
 ('reports_to')
),
(
 (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = 'Kavana'),
 (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = 'Rajni'),
 ('reports_to')
);


-- Query to get the manager of given employee 
SELECT e.EmployeeName, e.EmployeeID
FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID 
WHERE e.EmployeeID in ( SELECT e2.EmployeeID 
FROM Employee e1, supplies_to , Employee e2
WHERE MATCH(e1-(supplies_to)-> e2)
AND e1.EmployeeID in ( SELECT empo.EmployeeID 
FROM Employee empo JOIN EmployeeDetails ed ON empo.EmployeeID = ed.EmployeeID 
AND ed.EmployeeName = 'Souvik'))

-- Query to get the employee under a given manager
SELECT e.EmployeeName
FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID 
WHERE e.EmployeeID in ( SELECT e2.EmployeeID 
FROM Employee e1, supplies_to , Employee e2
WHERE MATCH(e2-(supplies_to)-> e1)
AND e1.EmployeeID in ( SELECT empo.EmployeeID 
FROM Employee empo JOIN EmployeeDetails ed ON empo.EmployeeID = ed.EmployeeID 
AND ed.EmployeeName = 'Rajni'))
