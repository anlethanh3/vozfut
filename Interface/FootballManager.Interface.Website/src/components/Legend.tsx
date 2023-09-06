import { Modal, Tab, Tabs, Button, Table } from 'react-bootstrap';

export default function Legend(props: { show: boolean, onClose: () => void, }) {
    const { show, onClose, } = props

    const legend = {
        details: [
            { key: "Giảm", description: "Đá dưới 04 trận/tháng, giảm trực tiếp -1 cho tất cả các chỉ số. Giảm tối đa 02 so với chỉ số gốc." },
            { key: "Tăng", description: "Đá trên 10 trận/tháng, tăng trực tiếp +1 cho tất cả các chỉ số. Tối đa là 5." },
            { key: "Tăng", description: "Tỉ lệ ghi bàn dựa trên số trận thi đấu của tháng trên 1.5, tăng trực tiếp +1 tất cả các chỉ số. Tối đa là 5." },
        ],
        stats: [
            { value: "80%", key: "Tốc độ", description: "Là khả năng phô diễn tốc độ của bản thân kể cả đoạn ngắn và đoạn dài." },
            { value: "80%", key: "Thể lực", description: "Là chỉ số về thể chất bao gồm khả năng tì đè, che chắn." },
            { value: "115%", key: "Sút", description: "Là chỉ số về khả năng sút, bao gồm cả lực sút." },
            { value: "115%", key: "Chuyền", description: "Là chỉ số về khả năng chuyền bóng, bao gồm khả năng thực hiện đường chuyền và chuyền chính xác." },
            { value: "110%", key: "Kỹ thuật", description: "Là chỉ số về khả năng khống chế bóng toàn diện, bao gồm cả lừa bóng và đi bóng." },
        ]
    }

    function ShowDetails() {
        var contents: JSX.Element[] = []
        legend.details.forEach((value, index) => {
            contents.push(
                <tr>
                    <td>{index + 1}</td>
                    <td key={`detail-key-${index}`}>{value.key}</td>
                    <td key={`detail-description-${index}`}>{value.description}</td>
                </tr>
            )
        })
        return (
            <Table striped hover responsive bordered>
                <thead>
                    <tr>
                        <th>No</th>
                        <th className='col-sm-1'>Tiêu chí</th>
                        <th>Chi tiết</th>
                    </tr>
                </thead>
                <tbody>{contents}</tbody>
            </Table>
        )
    }

    function ShowStats() {
        var contents: JSX.Element[] = []
        legend.stats.forEach((value, index) => {
            contents.push(
                <tr>
                    <td>{index + 1}</td>
                    <td key={`detail-key-${index}`}>{value.key}</td>
                    <td key={`detail-value-${index}`}>{value.value}</td>
                    <td key={`detail-description-${index}`}>{value.description}</td>
                </tr>
            )
        })
        return (
            <Table striped hover responsive bordered>
                <thead>
                    <tr>
                        <th>No</th>
                        <th className='col-sm-1'>Chỉ số</th>
                        <th className='col-sm-1'>Tỉ lệ</th>
                        <th>Chi tiết</th>
                    </tr>
                </thead>
                <tbody>{contents}</tbody>
            </Table>
        )
    }

    return (
        <>
            <Modal show={show} onHide={onClose} size='lg'>
                <Modal.Header closeButton>
                    <Modal.Title>Bảng công thức tính Elo</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Tabs
                        defaultActiveKey="stats"
                        id="uncontrolled-tab-example"
                        className="mb-3"
                    >
                        <Tab eventKey="stats" title="Cách tính Elo">
                            <ShowStats />
                        </Tab>
                        <Tab eventKey="boostup" title="Tiêu chuẩn đánh giá">
                            <ShowDetails />
                        </Tab>
                    </Tabs>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={() => { onClose() }}>Close</Button>
                </Modal.Footer>
            </Modal >
        </>
    )
}