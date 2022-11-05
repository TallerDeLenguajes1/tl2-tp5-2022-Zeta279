CREATE TABLE cadeteria
(
	id_cadeteria INT,
	nombre TEXT NOT NULL,
	CONSTRAINT pk_cadeteria PRIMARY KEY (id_cadeteria)
);

CREATE TABLE cadete
(
	id_cadete INT,
	nombre TEXT NOT NULL,
	direccion TEXT,
	telefono TEXT NOT NULL,
	id_cadeteria INT NOT NULL,
	CONSTRAINT pk_cadete PRIMARY KEY (id_cadete),
	CONSTRAINT fk_cadete_cadeteria FOREIGN KEY (id_cadeteria) REFERENCES cadeteria(id_cadeteria)
);

CREATE TABLE cliente
(
	id_cliente INT,
	nombre TEXT NOT NULL,
	direccion TEXT NOT NULL,
	telefono TEXT NOT NULL,
	CONSTRAINT pk_cliente PRIMARY KEY (id_cliente)
);

CREATE TABLE pedido
(
	id_pedido INT,
	observacion TEXT,
	id_cliente INT NOT NULL,
	id_cadete INT,
	CONSTRAINT pk_pedido PRIMARY KEY (id_pedido), 
	CONSTRAINT fk_pedido_cliente FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente),
	CONSTRAINT fk_pedido_cadete FOREIGN KEY (id_cadete) REFERENCES cadete(id_cadete)
);