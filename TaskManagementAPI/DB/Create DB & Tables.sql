-- Create the TaskDB database
CREATE DATABASE TaskDB;
GO

-- Switch to the TaskDB database
USE TaskDB;
GO

-- Create the TaskStatuses lookup table
CREATE TABLE TaskStatuses (
    StatusId INT PRIMARY KEY IDENTITY(1,1),
    StatusName NVARCHAR(50) UNIQUE NOT NULL
);

-- Insert initial status values
INSERT INTO TaskStatuses (StatusName)
VALUES ('Not Started'), ('In Progress'), ('Completed'), ('On Hold');

-- Create the TaskPriorities lookup table
CREATE TABLE TaskPriorities (
    PriorityId INT PRIMARY KEY IDENTITY(1,1),
    PriorityName NVARCHAR(50) UNIQUE NOT NULL
);

-- Insert initial priority values
INSERT INTO TaskPriorities (PriorityName)
VALUES ('High'), ('Medium'), ('Low');

-- Create the Tasks table with foreign keys to lookup tables
CREATE TABLE Tasks (
    TaskId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    PriorityId INT,
    DueDate DATE,
    StatusId INT,
    CONSTRAINT FK_Tasks_StatusId FOREIGN KEY (StatusId) REFERENCES TaskStatuses(StatusId),
    CONSTRAINT FK_Tasks_PriorityId FOREIGN KEY (PriorityId) REFERENCES TaskPriorities(PriorityId)
);


INSERT INTO Tasks (Title, Description, PriorityId, DueDate, StatusId)
VALUES
    ('Finish Marketing Proposal', 'Complete the proposal for the new campaign', 1, '2024-08-05', 2),   
    ('Schedule Team Meeting', 'Send out invites for next week''s meeting', 3, '2024-07-01', 1),   
    ('Update Website Content', 'Add new product descriptions and images', 2, '2024-07-10', 2), 
    ('Prepare Financial Report', 'Gather data and create Q2 report', 1, '2024-07-31', 3),     
    ('Research Competitors', 'Analyze competitor strategies and pricing', 3, '2024-08-15', 1),  
    ('Plan Product Launch Event', 'Organize logistics and create marketing materials', 2, '2024-09-01', 1),
    ('Conduct User Testing', 'Gather feedback on the new app features', 3, '2024-07-20', 2),   
    ('Create Social Media Content', 'Design posts for upcoming promotions', 2, '2024-07-05', 3),  
    ('Attend Industry Conference', 'Network with potential clients and partners', 3, '2024-08-28', 1),
    ('Analyze Sales Data', 'Identify trends and areas for improvement', 2, '2024-07-12', 3);   

ALTER TABLE Tasks
ADD IsDelete INT NOT NULL DEFAULT 0; 

