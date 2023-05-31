DROP TABLE IF EXISTS "Users" CASCADE;
DROP TABLE IF EXISTS "Ads" CASCADE;
DROP TABLE IF EXISTS "FavoriteAds" CASCADE;

create table "Users"(
	"Id" uuid primary key,
	"FirstName" varchar(50) not null,
	"LastName" varchar(50) not null,
	"City" varchar(50) not null,
	"Address" varchar(50) not null,
	"Phone" int not null,
	"Email" varchar(200) not null
);

create table "Ads"(
	"Id" uuid primary key not null,
	"UserId" uuid not null,
    "Content" varchar not null,
	"CreatedAt" date not null default current_date,
	CONSTRAINT FK_Ads_User
      FOREIGN key ("UserId") 
	  REFERENCES "Users"("Id")
);

create table "FavoriteAds"(
	"Id" serial primary key,
	"UserId" uuid not null,
	"AdId" uuid not null,
	CONSTRAINT FK_FavoriteAds_Users_UserId
	  FOREIGN KEY("UserId") 
	  REFERENCES "Users"("Id"),
	CONSTRAINT FK_FavoriteAds_Ads_AdId
      FOREIGN KEY("AdId") 
	  REFERENCES "Ads"("Id")
);

insert into "Users" ("Id", "FirstName", "LastName", "City", "Address", "Phone", "Email")
VALUES ((uuid_generate_v4()), 'Pero', 'Perić', 'Osijek', 'Ulica 1', 031234321, 'pero@mail.com');

insert into "Ads" ("Id", "UserId", "Content")
VALUES ((uuid_generate_v4()),(select "Id" from "Users" where "FirstName" = 'Pero'), 'Lorem ipsum...');

CREATE EXTENSION "uuid-ossp";

alter table Ads drop constraint if exists fk_user;
alter table FavoriteAds drop constraint if exists FK_FavoriteAds_Users_UserId;
alter table FavoriteAds drop constraint if exists FK_FavoriteAds_Ads_AdId;

alter table Users alter column Id set data type uuid using newid();
alter table users drop column Id

alter table users add column Id uuid;

update users set Id=uuid_generate_v4();

alter table users alter column Id set not null;

alter table ads alter column Id set data type uuid using newid();
alter table ads drop column Id;
alter table ads add column Id uuid;
update ads set Id=uuid_generate_v4();
alter table ads alter column Id set not null;

alter table ads drop column UserId;
alter table ads add column UserId uuid;
alter table ads alter column UserId set not null;

alter table "Ads" add column "UpdatedAt" timestamp ;

create table "Category"(
	"Id" serial primary key,
	"Name" varchar(50) not null,
	"Description" varchar(50) not null
);

create table "AdCategory"(
	"Id" uuid primary key not null ,
	"AdId" uuid not null,
	"CategoryId" int not null,
	CONSTRAINT FK_AdCategory_Ads_AdId
      FOREIGN KEY("AdId") 
	  REFERENCES "Ads"("Id"),
	CONSTRAINT FK_AdCategory_Category_AdId
      FOREIGN KEY("CategoryId") 
	  REFERENCES "Category"("Id")
);

insert into "Category"  ("Name", "Description")
VALUES ('Living', 'Living category');

with new_ad as (
  insert into "Ads"("Id", "UserId", "Content")
  values ((uuid_generate_v4()),(select "Id" from "Users" where "FirstName" = 'Pero'), 'Lorem ipsum...')
  returning "Id"
)
insert into "AdCategory" ("Id", "AdId", "CategoryId")
select (uuid_generate_v4()), "Id", 1
from new_ad;