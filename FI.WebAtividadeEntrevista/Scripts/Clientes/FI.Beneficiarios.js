function IncluirBeneficiario() {

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

    CarregarBeneficiario(cpf, nome)

    var beneficiario = {
        "Nome": nome,
        "CPF": cpf
    }

    beneficiarios.push(beneficiario)

    document.getElementById('CPFBeneficiario').value = ''
    document.getElementById('NomeBeneficiario').value = ''
}

function CarregarBeneficiarios(beneficiarios) {

    for (var i = 0; i < beneficiarios.length; i++) {
        CarregarBeneficiario(beneficiarios[i].CPF, beneficiarios[i].Nome)
    }
}

function CarregarBeneficiario(cpf, nome) {

    var table = document.getElementById("beneficiarioTable");

    var row = table.insertRow();

    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);

    cell1.innerHTML = cpf;
    cell2.innerHTML = nome;
    cell3.innerHTML = "<button type=\"button\" class=\"btn btn-sm btn-primary\">Alterar</button> <button type=\"button\" class=\"btn btn-sm btn-primary\">Excluir</button> ";
}

function AlterarBeneficiario() {

}

$(document).ready(function () {
    $('#CPFBeneficiario').mask('000.000.000-00');
});

function AbrirModalBenediciario(beneficiarios) {
    $('#modalBeneficiario').modal('show');
}