function IncluirBeneficiario() {

    var id = document.getElementById('IdBeneficiario').value || 0
    var cpf = document.getElementById('CPFBeneficiario').value || ''
    var nome = document.getElementById('NomeBeneficiario').value || ''

    if (!cpf) {
        ModalDialog('Beneficiarios', 'O campo CPF do beneficiario e obrigatorio')
        return
    }

    if (!nome) {
        ModalDialog('Beneficiarios', 'O campo nome do beneficiario e obrigatorio')
        return
    }

    CarregarBeneficiario(id, cpf, nome)

    var beneficiario = {
        "Id": id,
        "Nome": nome,
        "CPF": cpf
    }

    if (id > 0) {

        const index = beneficiarios.findIndex(item => item.Id == id);

        beneficiarios.splice(index, 1);

        beneficiarios.push(beneficiario)

        var table = document.getElementById("beneficiarioTable");

        table.innerHTML = ''

        CarregarBeneficiarios(beneficiarios)
    }
    else {
        beneficiarios.push(beneficiario)
    }

    document.getElementById('IdBeneficiario').value = 0
    document.getElementById('CPFBeneficiario').value = ''
    document.getElementById('NomeBeneficiario').value = ''
}

function CarregarBeneficiarios(beneficiariosCarregar) {

    for (var i = 0; i < beneficiariosCarregar.length; i++) {
        
        CarregarBeneficiario(beneficiariosCarregar[i].Id, beneficiariosCarregar[i].CPF, beneficiariosCarregar[i].Nome)
    }
}

function CarregarBeneficiario(id, cpf, nome) {

    var table = document.getElementById("beneficiarioTable");

    var row = table.insertRow();

    var cell0 = row.insertCell(0);
    var cell1 = row.insertCell(1);
    var cell2 = row.insertCell(2);
    var cell3 = row.insertCell(3);

    cell0.innerHTML = id
    cell1.innerHTML = cpf;
    cell2.innerHTML = nome;
    cell3.innerHTML = "<button onclick=\"CarregarDados(this)\" type=\"button\" class=\"btn btn-sm btn-primary\">Alterar</button> <button type=\"button\" class=\"btn btn-sm btn-primary\">Excluir</button> ";
}

function CarregarDados(button) {

    var row = button.parentNode.parentNode;

    var id = row.cells[0].innerHTML || '';
    var cpf = row.cells[1].innerHTML || '';
    var nome = row.cells[2].innerHTML || '';

    document.getElementById('IdBeneficiario').value = id
    document.getElementById('CPFBeneficiario').value = cpf
    document.getElementById('NomeBeneficiario').value = nome


    //if (!cpf) {
    //    ModalDialog('Beneficiarios', 'O campo CPF do beneficiario e obrigatorio')
    //    return
    //}

    //if (!nome) {
    //    ModalDialog('Beneficiarios', 'O campo nome do beneficiario e obrigatorio')
    //    return
    //}

    //const index = beneficiarios.findIndex(item => item.id === 2);

    //beneficiarios.splice(index, 1);

    //var beneficiario = {
    //    "Id": id,
    //    "Nome": nome,
    //    "CPF": cpf
    //}

    //beneficiarios.push(beneficiario)

    //table.innerHTML = ''

    //CarregarBeneficiarios(beneficiarios)
}

$(document).ready(function () {
    $('#CPFBeneficiario').mask('000.000.000-00');
});

function AbrirModalBenediciario(beneficiarios) {
    $('#modalBeneficiario').modal('show');
}