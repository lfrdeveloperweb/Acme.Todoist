CREATE TABLE todo 
(
	todo_id			varchar(32)		CONSTRAINT todo_pk PRIMARY KEY,
	title			varchar(128)	NOT NULL,
	project_id		varchar(32)		NULL CONSTRAINT todo_project_fk REFERENCES project,
	priority		int4			NOT NULL,
	due_date		date			NULL,
	completed_at	timestamptz		NULL,
	created_by		varchar(32)		NOT NULL,
	created_at		timestamptz		NOT NULL,
	description		varchar(512)	NULL,
	updated_at		timestamptz		NULL,
	updated_by		varchar(32)		NULL,
	deleted_at		timestamptz		NULL,
	labels			json			NULL
);

