Deployment Workflow
===================

Goals:

- Deploying solutions of SQL scripts with DDL to different environments of Servers.
- Flexible configuration of the deployment process and visual studio integration through MSBuild.
- Reduce SQL overhead for creating large programs in SQL language.
- Migrate existing databases and fresh install possible.

Migration
---------

Steps:

- Each script is assigned a version through the script mapping (semantic versioning number e.g. 1.5.2).
- Table named SqlBuildInfo contains version that is installed on the server.
