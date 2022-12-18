CREATE TABLE todo 
(
	todo_id			varchar(32)		CONSTRAINT todo_pk PRIMARY KEY,
	title			varchar(128)	NOT NULL,
	description		varchar(512)	NULL,
	project_id		varchar(32)		NULL CONSTRAINT todo_project_fk REFERENCES project,
	priority		int4			NOT NULL,
	labels			json			NULL,
	due_date		date			NULL,
	completed_at	timestamptz		NULL,
	created_by		varchar(32)		NOT NULL CONSTRAINT todo_user_created_by_fk REFERENCES "user",
	created_at		timestamptz		NOT NULL,	
	updated_by		varchar(32)		NULL  CONSTRAINT todo_user_updated_by_fk REFERENCES "user",
	updated_at		timestamptz		NULL,
	deleted_at		timestamptz		NULL	
);
