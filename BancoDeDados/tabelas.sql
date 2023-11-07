
CREATE DATABASE db_SistemaEscolar;

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

INSERT INTO nivel_acesso (descricao) VALUES ('Professor');
INSERT INTO nivel_acesso (descricao) VALUES ('Aluno');
SELECT * FROM nivel_acesso;


SELECT * FROM alunos;

--TRUNCATE TABLE alunos;

drop table nivel_acesso

INSERT INTO alunos (nome_aluno, cpf, tel, genero, estado_civil, nasc, cidade_nasc, estado_nasc, endereco, bairro, cidade, estado, numero, complemento, cep, email, senha, nivel) 
VALUES 
('Tiago Eugenio de Freitas', '341.274.748-31', '13 99119-2790', 'Masculino', 'Solteiro', '2023-11-01', 'Santos', 'SP', 'Rua Manoel Covas Raia', 'Vila São Jorge', 'São Vicente', 'SP', '193', 'casa', '11.380-070', 'tiago-vsj@hotmail.com', 'tiago@123', '2')


SELECT nome_aluno, cpf, tel, genero, estado_civil, nasc, cidade_nasc, estado_nasc, endereco, bairro, cidade, estado, numero, complemento, cep, email, senha FROM alunos WHERE cd_aluno = '1';