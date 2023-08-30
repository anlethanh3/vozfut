import Modal from 'react-bootstrap/Modal';
import { Container, ListGroup, Row, Col, Badge, Alert, Button } from 'react-bootstrap';
import { RollingProps } from '../slices/matchDetailSlice';
import { MatchProps } from '../slices/matchSlice';
import '../scss/custom.scss';

export default function Rivals(props: { show: boolean, rivals: RollingProps[], onSubmit: (rivals: RollingProps[]) => void, onClose: () => void, match: MatchProps }) {
    const { show, rivals, onClose, onSubmit, match } = props

    function Team(): JSX.Element {
        let items: JSX.Element[] = []
        let colors = ['danger', 'warning', 'secondary', 'primary']
        let names = ['Red', 'Banana', 'Orange', 'Blue',]
        rivals.forEach((value, index) => {
            var item = (
                <Col key={`key-${index}`}>
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
                </Col>
            )
            items.push(item)
        });


        return (<>{items}</>)
    }


    return (
        <>
            <Modal show={show} onHide={onClose} fullscreen keyboard={false} backdrop='static'>
                <Modal.Header closeButton>
                    <Modal.Title>Team Division Rivals</Modal.Title>
                </Modal.Header>
                <Modal.Body >
                    <Container className='h-100' style={{
                        backgroundImage: `url("../rivals.png")`,
                        backgroundSize: '50% ',
                        backgroundPosition: 'top',
                        backgroundRepeat: 'no-repeat',
                    }}>
                        <Row style={{ opacity: '0.85' }}>
                            <Team />
                        </Row>
                        <Alert variant='info' className='mt-2' style={{ opacity: '0.85' }}>
                            <h1>{match.name}</h1>
                            <h3>{match.description}</h3>
                        </Alert>
                    </Container>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" onClick={() => { onSubmit(rivals) }}>Save Rivals</Button>
                </Modal.Footer>
            </Modal>
        </>
    )
}