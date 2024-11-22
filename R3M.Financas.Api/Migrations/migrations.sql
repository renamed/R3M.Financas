CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE TABLE categories (
        id uuid NOT NULL,
        name character varying(20) NOT NULL,
        parent_id uuid,
        insert_date timestamp with time zone,
        updaten_date timestamp with time zone,
        CONSTRAINT pk_categories PRIMARY KEY (id),
        CONSTRAINT fk_categories_categories_parent_id FOREIGN KEY (parent_id) REFERENCES categories (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE TABLE institutions (
        id uuid NOT NULL,
        name character varying(20) NOT NULL,
        initial_balance numeric NOT NULL,
        balance numeric NOT NULL,
        insert_date timestamp with time zone,
        updaten_date timestamp with time zone,
        CONSTRAINT pk_institutions PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE TABLE periods (
        id uuid NOT NULL,
        start date NOT NULL,
        "end" date NOT NULL,
        description character(6) NOT NULL,
        insert_date timestamp with time zone,
        updaten_date timestamp with time zone,
        CONSTRAINT pk_periods PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE TABLE movimentations (
        id uuid NOT NULL,
        date date NOT NULL,
        description character varying(30) NOT NULL,
        value numeric NOT NULL,
        category_id uuid NOT NULL,
        period_id uuid NOT NULL,
        institution_id uuid NOT NULL,
        insert_date timestamp with time zone,
        updaten_date timestamp with time zone,
        CONSTRAINT pk_movimentations PRIMARY KEY (id),
        CONSTRAINT fk_movimentations_categories_category_id FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE RESTRICT,
        CONSTRAINT fk_movimentations_institutions_institution_id FOREIGN KEY (institution_id) REFERENCES institutions (id) ON DELETE RESTRICT,
        CONSTRAINT fk_movimentations_periods_period_id FOREIGN KEY (period_id) REFERENCES periods (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE UNIQUE INDEX ix_categories_name ON categories (name);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE INDEX ix_categories_parent_id ON categories (parent_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE UNIQUE INDEX ix_institutions_name ON institutions (name);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE INDEX ix_movimentations_category_id ON movimentations (category_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE INDEX ix_movimentations_institution_id ON movimentations (institution_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE INDEX ix_movimentations_period_id ON movimentations (period_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    CREATE UNIQUE INDEX ix_periods_description ON periods (description);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20241122145926_InitialMigration') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20241122145926_InitialMigration', '8.0.10');
    END IF;
END $EF$;
COMMIT;

