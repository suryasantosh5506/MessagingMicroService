#!/bin/bash

echo "Starting SQL Server..."

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

echo "Waiting for SQL Server to start..."

# Wait until SQL Server accepts connections
until /opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "Santosh@123" \
    -N \
    -C \
    -Q "SELECT 1" > /dev/null 2>&1
do
    sleep 2
done

echo "SQL Server is ready."

# Check whether the database already exists
DATABASE_EXISTS=$(
    /opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "Santosh@123" \
    -N \
    -C \
    -h -1 \
    -W \
    -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.databases WHERE name = 'MessagingMicroservice';"
)

if [ "$DATABASE_EXISTS" -eq 0 ]; then

    echo "Database does not exist. Restoring..."

    /opt/mssql-tools18/bin/sqlcmd \
        -S localhost \
        -U sa \
        -P "Santosh@123" \
        -N \
        -C \
        -Q "
        RESTORE DATABASE MessagingMicroservice
        FROM DISK = '/backup/MessagingMicroservice.bak'
        WITH
            MOVE 'MessagingMicroservice'
                TO '/var/opt/mssql/data/MessagingMicroservice.mdf',
            MOVE 'MessagingMicroservice_log'
                TO '/var/opt/mssql/data/MessagingMicroservice_log.ldf';
        "

    echo "Database restore completed."

else

    echo "MessagingMicroservice database already exists. Skipping restore."

fi

echo "Database initialization completed."

# Keep SQL Server process alive
wait