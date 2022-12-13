CREATE TABLE project 
(
	project_id		varchar(32)		CONSTRAINT project_pk PRIMARY KEY,
	title			varchar(128)	NOT NULL,
	color			varchar(7)		NOT NULL,
	created_by		varchar(32)		NOT NULL,
	created_at		timestamptz		NOT NULL
);