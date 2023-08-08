import { useState } from "react"
import { Button, Accordion, Form, FormGroup, FloatingLabel } from "react-bootstrap";

const SearchMember = () => {

    const [name, setName] = useState('')

    const onChanged = (e) => {
        setName(e.target.value)
    }

    return (
        <Accordion className="my-2" defaultActiveKey="0">
            <Accordion.Item eventKey="0">
                <Accordion.Header>Search box</Accordion.Header>
                <Accordion.Body>
                    <Form>
                        <FormGroup className="mb-3" controlId="addMember.Name">
                            <FloatingLabel controlId="floatingName" label="Name">
                                <Form.Control type="text" placeholder="Input name" value={name} onChange={onChanged} />
                            </FloatingLabel>
                        </FormGroup>
                    </Form>
                    <Button className="me-2" variant="primary" onClick={null}>
                        Search
                    </Button>
                    <Button variant="secondary" onClick={null}>
                        Clear
                    </Button>
                </Accordion.Body>
            </Accordion.Item>
        </Accordion>
    )
}

export default SearchMember