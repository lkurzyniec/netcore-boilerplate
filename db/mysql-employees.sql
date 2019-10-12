--  Sample employee database
--  See changelog table for details
--  Copyright (C) 2007,2008, MySQL AB
--
--  Original data created by Fusheng Wang and Carlo Zaniolo
--  http://www.cs.aau.dk/TimeCenter/software.htm
--  http://www.cs.aau.dk/TimeCenter/Data/employeeTemporalDataSet.zip
--
--  Current schema by Giuseppe Maxia
--  Data conversion from XML to relational by Patrick Crews
--
-- This work is licensed under the
-- Creative Commons Attribution-Share Alike 3.0 Unported License.
-- To view a copy of this license, visit
-- http://creativecommons.org/licenses/by-sa/3.0/ or send a letter to
-- Creative Commons, 171 Second Street, Suite 300, San Francisco,
-- California, 94105, USA.
--
--  DISCLAIMER
--  To the best of our knowledge, this data is fabricated, and
--  it does not correspond to real people.
--  Any similarity to existing people is purely coincidental.
--

DROP DATABASE IF EXISTS employees;
CREATE DATABASE IF NOT EXISTS employees;
USE employees;

SELECT 'CREATING DATABASE STRUCTURE' as 'INFO';

DROP TABLE IF EXISTS employees,
                     departments;

/*!50503 set default_storage_engine = InnoDB */;
/*!50503 select CONCAT('storage engine: ', @@default_storage_engine) as INFO */;

CREATE TABLE employees (
    emp_no      INT             NOT NULL,
    birth_date  DATE            NOT NULL,
    first_name  VARCHAR(14)     NOT NULL,
    last_name   VARCHAR(16)     NOT NULL,
    gender      ENUM ('M','F')  NOT NULL,
    PRIMARY KEY (emp_no)
);

INSERT INTO employees VALUES
    (1,'1972-05-13','Peter','Diaz','M'),
    (2,'1987-09-25','Leon','Leonard','M'),
    (3,'1974-05-10','Shirley','Baker','F'),
    (4,'1986-08-17','David','Allen','M'),
    (5,'1959-10-14','Nancy','Davis','F'),
    (6,'1964-07-05','Michael','Wray','M'),
    (7,'1980-10-08','Wanda','Inniss','F');

CREATE TABLE departments (
    dept_no     CHAR(4)         NOT NULL,
    dept_name   VARCHAR(40)     NOT NULL,
    manger_no   INT             NOT NULL,
    PRIMARY KEY (dept_no),
    UNIQUE  KEY (dept_name),
    FOREIGN KEY (manger_no)       REFERENCES employees (emp_no)    ON DELETE CASCADE
);

INSERT INTO departments VALUES
    ('d001','Marketing', 3),
    ('d002','Finance', 1),
    ('d003','Human Resources', 7);

ALTER TABLE employees
    ADD COLUMN dept_no CHAR(4),
    ADD FOREIGN KEY (dept_no) REFERENCES departments(dept_no) ON DELETE CASCADE;

UPDATE employees
    SET dept_no = 'd001'
    WHERE emp_no IN (3);
UPDATE employees
    SET dept_no = 'd002'
    WHERE emp_no IN (1, 2);
UPDATE employees
    SET dept_no = 'd003'
    WHERE emp_no IN (4, 5, 6, 7);

CREATE USER 'user'@'localhost' IDENTIFIED WITH mysql_native_password BY 'simplepwd';
GRANT ALL ON employees.* TO 'user'@'localhost';

flush /*!50503 binary */ logs;
