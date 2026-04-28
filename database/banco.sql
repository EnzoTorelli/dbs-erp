CREATE DATABASE IF NOT EXISTS `techpoint informatica`;
USE `techpoint informatica`;

SET FOREIGN_KEY_CHECKS=0;

DROP TABLE IF EXISTS `categoria`;
CREATE TABLE `categoria` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
);
INSERT INTO `categoria` VALUES (1,'RAM'),(2,'SSD'),(3,'Processador'),(4,'Placa de Video');

DROP TABLE IF EXISTS `cliente`;
CREATE TABLE `cliente` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) NOT NULL,
  `cpf` varchar(14) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `telefone` varchar(20) DEFAULT NULL,
  `data_cadastro` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
);
INSERT INTO `cliente` VALUES (1,'João Silva','123.456.789-00','joao@email.com','(19)98888-8888','2026-04-13 22:02:32');

DROP TABLE IF EXISTS `empresa`;
CREATE TABLE `empresa` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) NOT NULL,
  `cnpj` varchar(20) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `telefone` varchar(20) DEFAULT NULL,
  `data_cadastro` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
);
INSERT INTO `empresa` VALUES 
(1,'TechPoint Informatica','00.000.000/0001-00','contato@techpoint.com','(19)99999-9999','2026-04-13 22:02:31'),
(2,'TechPoint Informatica','12.345.678/0001-99','contato@techpoint.com','(19)99999-9999','2026-04-13 22:05:03');

DROP TABLE IF EXISTS `pedido`;
CREATE TABLE `pedido` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_cliente` int DEFAULT NULL,
  `data_pedido` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `status` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
);

DROP TABLE IF EXISTS `produto`;
CREATE TABLE `produto` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) NOT NULL,
  `descricao` text,
  `preco` decimal(10,2) NOT NULL,
  `estoque` int DEFAULT '0',
  `id_categoria` int DEFAULT NULL,
  PRIMARY KEY (`id`)
);
INSERT INTO `produto` VALUES 
(1,'Memoria RAM 8GB','DDR4',199.90,10,1),
(2,'RTX 3060','Placa de video NVIDIA',2500.00,5,1),
(4,'Desktop Dell Pro Micro','Windows 11 Home / Intel Core i5 14500T / 8 GB DDR5 / 256GB SSD',6899.00,5,2),
(6,'Mouse Gamer REDRAGON','7 Botoes',158.99,5,3);

DROP TABLE IF EXISTS `item_pedido`;
CREATE TABLE `item_pedido` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_pedido` int DEFAULT NULL,
  `id_produto` int DEFAULT NULL,
  `quantidade` int NOT NULL,
  `preco_unitario` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`id`)
);

SET FOREIGN_KEY_CHECKS=1;