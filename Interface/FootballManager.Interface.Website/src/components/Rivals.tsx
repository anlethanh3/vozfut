import Modal from 'react-bootstrap/Modal';
import { Container, ListGroup, Row, Col, Badge, } from 'react-bootstrap';
import { RollingProps } from '../slices/matchDetailSlice';

export default function Rivals(props: { show: boolean, rivals: RollingProps[], onClose: () => void }) {
    const { show, rivals, onClose } = props

    function Team(): JSX.Element {
        let items: JSX.Element[] = []
        let colors = ['warning', 'primary', 'danger', 'success']
        let names = ['Yellow', 'Blue', 'Red', 'Orange']
        rivals.forEach((value, index) => {
            var item = (
                <Col>
                    <ListGroup>
                        <ListGroup.Item style={{ fontWeight: 'bold' }} variant={colors[index]}>Team {names[index]}</ListGroup.Item>
                        {
                            value.players.map((v, i) =>
                                <ListGroup.Item variant={colors[index]} as="li"
                                    className="d-flex justify-content-between align-items-start" >
                                    <div className="ms-2 me-auto">
                                        {i + 1}. {v.name}
                                    </div>
                                    <Badge bg={colors[index]} pill>
                                        +{v.elo}
                                    </Badge>
                                </ListGroup.Item>
                            )
                        }
                        <ListGroup.Item style={{ fontWeight: 'bold' }} variant={colors[index]} as="li"
                            className="d-flex justify-content-between align-items-start" >
                            <div className="ms-2 me-auto">
                                Elo Sum
                            </div>
                            <Badge bg={colors[index]} pill>
                                +{value.eloSum}
                            </Badge>
                        </ListGroup.Item>
                    </ListGroup>
                </Col >
            )
            items.push(item)
        });


        return (<>{items}</>)
    }


    return (
        <>
            <Modal show={show} onHide={onClose} fullscreen>
                <Modal.Header closeButton>
                    <Modal.Title>Team Division Rivals</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Container>
                        <Row>
                            <Team />
                        </Row>
                    </Container>
                </Modal.Body>
                <Modal.Footer>
                </Modal.Footer>
            </Modal>
        </>
    )
}