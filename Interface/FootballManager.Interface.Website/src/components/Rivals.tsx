import Modal from 'react-bootstrap/Modal';
import { Container, ListGroup, Row, Col, Badge, Alert, Button } from 'react-bootstrap';
import { RollingProps } from '../slices/matchDetailSlice';
import { MatchProps } from '../slices/matchSlice';
import '../scss/custom.scss';
interface TeamRivalInfo {
    color: string,
    name: string,
}
export default function Rivals(props: { show: boolean, rivals: RollingProps[], onSubmit: (rivals: RollingProps[]) => void, onClose: () => void, match: MatchProps }) {
    const { show, rivals, onClose, onSubmit, match } = props

    function Team(): JSX.Element {
        let items: JSX.Element[] = []
        let infos: TeamRivalInfo[] = [
            { color: 'warning', name: 'Banana' },
            { color: 'primary', name: 'Blue' },
            { color: 'danger', name: 'Red' },
            { color: 'secondary', name: 'Orange' },
        ]
        rivals.forEach((value, index) => {
            var item = (
                <Col key={`key-${index}`}>
                    <ListGroup>
                        <ListGroup.Item style={{ fontWeight: 'bold' }} variant={infos[index].color}>Team {infos[index].name}</ListGroup.Item>
                        {
                            value.players.map((v, i) =>
                                <ListGroup.Item variant={infos[index].color} as="li"
                                    className="d-flex justify-content-between align-items-start" >
                                    <div className="ms-2 me-auto">
                                        {i + 1}. {v.name}
                                    </div>
                                    <Badge bg={infos[index].color} pill>
                                        +{v.elo}
                                    </Badge>
                                </ListGroup.Item>
                            )
                        }
                        <ListGroup.Item style={{ fontWeight: 'bold' }} variant={infos[index].color} as="li"
                            className="d-flex justify-content-between align-items-start" >
                            <div className="ms-2 me-auto">
                                Elo Sum
                            </div>
                            <Badge bg={infos[index].color} pill>
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