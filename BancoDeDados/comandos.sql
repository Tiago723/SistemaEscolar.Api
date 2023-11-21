
use db_SistemaEscolar

SELECT * FROM nivel_acesso;


SELECT * FROM alunos;

--TRUNCATE TABLE alunos;

--drop table nivel_acesso

INSERT INTO alunos (nome_aluno, cpf, tel, genero, estado_civil, nasc, cidade_nasc, estado_nasc, endereco, bairro, cidade, estado, numero, complemento, cep, email, senha, nivel) 
VALUES 
('Tiago Eugenio de Freitas', '341.274.748-31', '13 99119-2790', 'Masculino', 'Solteiro', '2023-11-01', 'Santos', 'SP', 'Rua Manoel Covas Raia', 'Vila São Jorge', 'São Vicente', 'SP', '193', 'casa', '11.380-070', 'tiago-vsj@hotmail.com', 'tiago@123', '1')


SELECT nome_aluno, cpf, tel, genero, estado_civil, nasc, cidade_nasc, estado_nasc, endereco, bairro, cidade, estado, numero, complemento, cep, email, senha FROM alunos WHERE cd_aluno = '1';