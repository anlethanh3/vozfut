import Modal from 'react-bootstrap/Modal';
import { Container, ListGroup, Row, Col, Badge, Alert, Button } from 'react-bootstrap';
import { RivalScheduleProps, RollingProps } from '../slices/matchDetailSlice';
import { MatchProps } from '../slices/matchSlice';
import '../scss/custom.scss';
import { useCallback, useRef } from 'react';
import { toPng, } from 'html-to-image';
import RivalSchedule from './RivalSchedule';

export default function Rivals(props: { show: boolean, rivals: RollingProps[], onSubmit: (rivals: RollingProps[]) => void, onClose: () => void, match: MatchProps | undefined }) {
    const { show, rivals, onClose, onSubmit, match } = props
    const ref = useRef<HTMLDivElement>(null);
    const onButtonClick = useCallback(() => {
        if (ref.current === null) {
            return
        }
        toPng(ref.current, { cacheBust: true, width: 2000, height: 1200, backgroundColor: 'white' })
            .then((dataUrl) => {
                const link = document.createElement('a')
                link.download = 'image.png'
                link.href = dataUrl
                link.click()
            })
            .catch((err) => {
                console.log(err)
            })
    }, [ref])

    function teamColors(teamCount: number) {
        if (teamCount < 3) {
            return [
                { color: 'dark', name: 'White' },
                { color: 'secondary', name: 'Yellow' },
            ]
        }
        if (teamCount < 4) {
            return [
                { color: 'secondary', name: 'Yellow' },
                { color: 'primary', name: 'Blue' },
                { color: 'dark', name: 'White' },
            ]
        }
        return [
            { color: 'danger', name: 'Red' },
            { color: 'primary', name: 'Blue' },
            { color: 'dark', name: 'White' },
            { color: 'secondary', name: 'Yellow' },
        ]
    }
    function Team(): JSX.Element {
        let items: JSX.Element[] = []
        let infos = teamColors(match?.teamCount ?? 3)
        rivals.forEach((value, index) => {
            var item = (
                <Col key={`key-${index}`}>
                    <ListGroup key={`key-group-${index}`}>
                        <ListGroup.Item key={`key-list-${index}`} style={{ fontWeight: 'bold' }} variant={infos[index].color}>Team {infos[index].name}</ListGroup.Item>
                        {
                            value.players.map((v, i) =>
                                <ListGroup.Item key={`key-item-${index}`} variant={infos[index].color} as="li"
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
                        <ListGroup.Item key={`key-sum-${index}`} style={{ fontWeight: 'bold' }} variant={infos[index].color} as="li"
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

    function getSchedule(teamCount: number) {
        let schedules: RivalScheduleProps[] = [
            {
                homeMatches: [
                    { home: 0, away: 1 },
                ],
                awayMatches: [
                    { home: 1, away: 0 },
                ]
            },
            {
                homeMatches: [
                    { home: 2, away: 0 },
                    { home: 1, away: 0 },
                    { home: 1, away: 2 },
                    { home: 0, away: 2 },
                    { home: 0, away: 1 },
                ],
                awayMatches: [
                    { home: 1, away: 2 },
                    { home: 0, away: 2 },
                    { home: 0, away: 1 },
                    { home: 2, away: 1 },
                ]
            },
            {
                homeMatches: [
                    { home: 3, away: 1 },
                    { home: 0, away: 2 },
                    { home: 0, away: 3 },
                    { home: 1, away: 2 },
                    { home: 3, away: 2 },
                    { home: 1, away: 0 },
                ],
                awayMatches: [
                    { home: 3, away: 2 },
                    { home: 1, away: 0 },
                    { home: 1, away: 2 },
                    { home: 3, away: 0 },
                    { home: 3, away: 1 },
                    { home: 0, away: 2 },
                ],
            }]
        if (teamCount > 3) {
            return schedules[2]
        }
        return teamCount > 2 ? schedules[1] : schedules[0]
    }

    return (
        <>
            <Modal show={show} onHide={onClose} fullscreen backdrop='static'>
                <Modal.Header closeButton>
                    <Modal.Title>Team Division Rivals</Modal.Title>
                </Modal.Header>
                <Modal.Body ref={ref}>
                    <Container style={{
                        backgroundImage: `url("../rivals.png")`,
                        backgroundPosition: 'top',
                        backgroundRepeat: 'no-repeat',
                    }}>
                        <Row style={{ opacity: '0.85', padding: '2% 0%' }}>
                            <Team />
                        </Row>
                        <Alert variant='info' className='mt-2' style={{ opacity: '0.85' }}>
                            <h1>{match?.name}</h1>
                            <h3>{match?.description}</h3>
                        </Alert>
                        {
                            match &&
                            <RivalSchedule data={getSchedule(match.teamCount)} match={match} />
                        }
                    </Container>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" onClick={() => { onButtonClick() }}>Save as Image</Button>
                </Modal.Footer>
            </Modal >
        </>
    )
}