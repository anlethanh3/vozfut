import { Image, Table } from "react-bootstrap";

export default function Donate() {
    return (
        <>
            <h1>Donate</h1>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Quỹ Momo đội bóng Vẩu Zơ</th>
                        <th>Chuyển khoản vào ACB thầy Phong</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><Image src={'donate-momo.png'}/></td>
                        <td><Image src={'donate-private.png'}/></td>
                    </tr>
                </tbody>
            </Table>
        </>
    )
}