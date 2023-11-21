
--CREATE DATABASE db_SistemaEscolar;

USE db_SistemaEscolar;

CREATE TABLE nivel_acesso(
	nivel INT NOT NULL IDENTITY PRIMARY KEY,
	descricao VARCHAR(85) NOT NULL,
);

CREATE TABLE alunos(
	cd_aluno INT NOT NULL IDENTITY PRIMARY KEY,
	nome_aluno VARCHAR(85) NOT NULL,
	cpf VARCHAR(15) NOT NULL UNIQUE,
	tel VARCHAR(15) NOT NULL,
	genero VARCHAR(25) NOT NULL,
	estado_civil VARCHAR(15) NOT NULL,
	nasc VARCHAR(15) NOT NULL,
	cidade_nasc VARCHAR(85) NOT NULL,
	estado_nasc VARCHAR(85) NOT NULL,
	endereco VARCHAR(85) NOT NULL,
	bairro VARCHAR(85) NOT NULL,
	cidade VARCHAR(85) NOT NULL,
	estado VARCHAR(85) NOT NULL,
	numero VARCHAR(15) NOT NULL,
	complemento VARCHAR(15) NOT NULL,
	cep VARCHAR(12) NOT NULL,
	email VARCHAR(85) NOT NULL UNIQUE,
	senha NVARCHAR(MAX) NOT NULL,
	nivel INT NOT NULL,
	foreign key (nivel)
	references nivel_acesso(nivel)
);

INSERT INTO nivel_acesso (descricao) VALUES ('Aluno');
INSERT INTO nivel_acesso (descricao) VALUES ('Professor');
INSERT INTO nivel_acesso (descricao) VALUES ('Diretoria');

